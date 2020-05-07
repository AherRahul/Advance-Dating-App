import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { CommonModule } from '@angular/common';
import { User } from 'src/app/_model/user';

@Component({
  selector: 'app-model',
  templateUrl: './model.component.html',
  styleUrls: ['./model.component.css']
})
export class ModelComponent implements OnInit {

  @Output() updateSelectedRole = new EventEmitter();
  user: User;
  roles: any[];

  constructor(public bsModalRef: BsModalRef) { }

  ngOnInit() {
  }

  updateRoles() {
    this.updateSelectedRole.emit(this.roles);
    this.bsModalRef.hide();
  }
}
