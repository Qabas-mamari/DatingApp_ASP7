import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AccountService } from './_services/account.service';
import { User } from './_models/user';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent implements OnInit {
  title = 'Dating App';
  users: any;

  // The constructor is used for initializing the object and setting its internal state.
  // The "http" parameter is automatically assigned to the private property.
  constructor(private http: HttpClient, private accountService: AccountService) { }

  ngOnInit(): void {
    this.getUsers();
    this.setCurrentUser();
  }

  getUsers() {
    //1. The subscribe method is called on the Observable to start listening for the response
    this.http.get('https://localhost:5001/api/users').subscribe({
      next: response => this.users = response, // when the request is successful, and it receives the response from the server.
      error: error => console.log(error), //  if an error occurs during the request
      complete: () => console.log("Request is complete") // when the request is complete, regardless of success or failure.
    })
  }

  /**
   * TypeScript method retrieves the user information from the localStorage and sets it as the current user y invoking the setCurrentUser method of an accountService.
   */
  setCurrentUser() {
    const userString = localStorage.getItem('user');
    if (!userString) return;
    const user: User = JSON.parse(userString);
    this.accountService.setCurrentUser(user);
  }

}
