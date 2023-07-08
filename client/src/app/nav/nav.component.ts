import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};
  //loggedIn = false;

  constructor(public accountService: AccountService, private routes: Router) { }

  ngOnInit(): void {
  }

  // getCurrentUser() {
  //   this.accountService.currentUser$.subscribe({
  //     next: user => this.loggedIn = !!user,
  //     error: error => console.log(error)
  //   })
  // }

  login() {
    this.accountService.login(this.model).subscribe({
      // next: response => {
      //   console.log(response);
      //  // this.loggedIn = true;
      // },
      next: _=> this.routes.navigateByUrl("/members"),
      error: error => console.log(error)
    })
  }

  /**
   * TypeScript method logs out the current user by invoking the logout method of an accountService and setting the loggedIn property to false
   */
  logout() {
    this.accountService.logout();
    this.routes.navigateByUrl("/");
   // this.loggedIn = false;
  }
}
