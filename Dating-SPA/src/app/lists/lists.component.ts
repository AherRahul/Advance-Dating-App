import { Component, OnInit } from '@angular/core';
import { User } from '../_model/user';
import { Pagination, PaginatedResult } from '../_model/pagination';
import { AuthService } from '../_services/auth.service';
import { UserService } from '../_services/user.service';
import { ActivatedRoute } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit {
  users: User[];
  pagination: Pagination;
  likeParams: string;

  constructor(
    private authService: AuthService,
    private userService: UserService,
    private routes: ActivatedRoute,
    private alertify: AlertifyService
  ) { }

  ngOnInit() {
    this.routes.data.subscribe(data => {
      this.users = data.users.result;
      this.pagination = data.users.pagination;
    });

    this.likeParams = 'Likers';
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadUsers();
  }

  loadUsers() {
    this.userService.getUsers(this.pagination.currentPage, this.pagination.itemsPerPage, null, this.likeParams)
      .subscribe((res: PaginatedResult<User[]>) => {
        this.users = res.result;
        this.pagination = res.pagination;
    }, error => {
      this.alertify.error(error);
    });
  }

}
