import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  registerMode = false;
  users: any;

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.getUsers();
  }

  RegisterToggle() {
    this.registerMode = !this.registerMode;
  }

  getUsers() {
    //1. The subscribe method is called on the Observable to start listening for the response
    this.http.get('https://localhost:5001/api/users').subscribe({
      next: response => this.users = response, // when the request is successful, and it receives the response from the server.
      error: error => console.log(error), //  if an error occurs during the request
      complete: () => console.log("Request is complete") // when the request is complete, regardless of success or failure.
    })
  }

  cancelRegistrationMode(event: boolean){
    this.registerMode= event;
  }

}
