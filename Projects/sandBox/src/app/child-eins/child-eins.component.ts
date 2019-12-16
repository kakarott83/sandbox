import { Component, OnInit } from '@angular/core';
import { User } from '../user';
import { FormGroup, FormControl } from '@angular/forms';
import { DataService } from '../data.service';

@Component({
  selector: 'app-child-eins',
  templateUrl: './child-eins.component.html',
  styleUrls: ['./child-eins.component.scss']
})
export class ChildEinsComponent implements OnInit {

  myFormGroup: FormGroup;
  user: User;

  constructor(private userService: DataService) {
    this.myFormGroup = this.createFormGroup();
  }

  ngOnInit() {
  }

  onSubmit() {
    console.log(this.myFormGroup.value);
    this.userService.addUser(this.myFormGroup.value);
  }

  createFormGroup() {
    return new FormGroup({
      name: new FormControl(''),
      email: new FormControl(''),
    });
  }

}
