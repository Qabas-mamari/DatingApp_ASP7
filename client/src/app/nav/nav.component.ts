import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};
  //loggedIn = false;

  constructor(public accountService: AccountService, private routes: Router, private toastr: ToastrService) { }

  ngOnInit(): void {
  }

  login() {
    this.accountService.login(this.model).subscribe({
      next: _=> this.routes.navigateByUrl("/members"),
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
