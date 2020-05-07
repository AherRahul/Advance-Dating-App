import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/_model/user';
import { AdminService } from 'src/app/_services/admin.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { ModelComponent } from '../model/model.component';
import { __values } from 'tslib';
import { error } from '@angular/compiler/src/util';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.css']
})
export class UserManagementComponent implements OnInit {

  bsModalRef: BsModalRef;
  users: User[];

  constructor(
    private adminService: AdminService,
    private alertify: AlertifyService,
    private modalService: BsModalService,
    private authService: AuthService
  ) { }

  ngOnInit() {
    if (this.authService.roleMatch(['Admin'])) {
      this.getUsersWithRoles();
    }
  }

  getUsersWithRoles() {
    this.adminService.getUsersWithRoles().subscribe((users: User[]) => {
      this.users = users;
    // tslint:disable-next-line: no-shadowed-variable
    }, error => {
      this.alertify.error(error);
    });
  }

  openModalWithComponent(user: User) {
    const initialState = {
      user,
      roles: this.getRoleArray(user)
    };
    this.bsModalRef = this.modalService.show(ModelComponent, {initialState});
    this.bsModalRef.content.updateSelectedRole.subscribe((values) => {
      const rolesToUpdate = {
        // Spreds operator ...
        roleNames: [...values.filter(el => el.checked === true).map(el => el.name)]
      };
      if (rolesToUpdate) {
        this.adminService.updateUserRoles(user, rolesToUpdate).subscribe(() => {
          user.roles = [...rolesToUpdate.roleNames];
        // tslint:disable-next-line: no-shadowed-variable
        }, error => {
          console.log(error);
        });
      }
    });
  }

  private getRoleArray(user: User) {
    const roles = [];
    const userRoles = user.roles;
    const availableRoles: any[] = [
      {name: 'Admin', value: 'Admin'},
      {name: 'Moderator', value: 'Moderator'},
      {name: 'Member', value: 'Member'},
      {name: 'VIP', value: 'VIP'}
    ];

    // tslint:disable-next-line: prefer-for-of
    for (let i = 0; i < availableRoles.length; i++) {
      let isMatch = false;

      // tslint:disable-next-line: prefer-for-of
      for (let j = 0; j < userRoles.length; j++) {
        if (availableRoles[i].name === userRoles[j]) {
          isMatch = true;
          availableRoles[i].checked = true;
          roles.push(availableRoles[i]);
          break;
        }
      }

      if (!isMatch) {
        availableRoles[i].checked = false;
        roles.push(availableRoles[i]);
      }
    }
    return roles;
  }
}
