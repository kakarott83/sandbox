import { Injectable } from '@angular/core';

import { User } from './user';
import { Observable, of, BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DataService {

  users: BehaviorSubject<User[]> = new BehaviorSubject<User[]>([
    {name: 'Michael', email: 'test@1.de'},
    {name: 'Philipp', email: 'test@2.de'}
  ]);


  public get users$(): Observable<User[]> {
    return this.users;
  }

  constructor() {
  }

  addUser(user: User) {
    const newUsers = [
      ...this.users.getValue(),
      user
    ];



    this.users.next(newUsers);
  }




}
