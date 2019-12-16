import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { DataService } from '../data.service';
import { User } from '../user';
import { Observable } from 'rxjs';
import { debounceTime } from 'rxjs/operators';

@Component({
  selector: 'app-child-zwei',
  templateUrl: './child-zwei.component.html',
  styleUrls: ['./child-zwei.component.scss'],
})
export class ChildZweiComponent implements OnInit {

  public users$: Observable<User[]>;

  constructor(private userService: DataService) {
  }

  ngOnInit() {
    // this.users$ = this.userService.users$.pipe(debounceTime(10000));
    this.users$ = this.userService.users$;
  }


}
