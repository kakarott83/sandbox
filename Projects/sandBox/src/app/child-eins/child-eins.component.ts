import { Component, OnInit } from '@angular/core';
import { User } from '../user';
import { FormGroup, FormControl } from '@angular/forms';
import { DataService } from '../data.service';
import * as moment from 'moment';

@Component({
  selector: 'app-child-eins',
  templateUrl: './child-eins.component.html',
  styleUrls: ['./child-eins.component.scss']
})
export class ChildEinsComponent implements OnInit {

  myFormGroup: FormGroup;
  user: User;
  selected: {startDate: moment.Moment, endDate: moment.Moment};

  constructor(private userService: DataService) {
    this.myFormGroup = this.createFormGroup();
  }

  ngOnInit() {
  }

  onSubmit() {
    console.log(this.selected.startDate.date);
    this.userService.addUser(this.myFormGroup.value);
  }

  createFormGroup() {
    return new FormGroup({
      name: new FormControl(''),
      email: new FormControl(''),
      date: new FormControl(''),
    });
  }

}
