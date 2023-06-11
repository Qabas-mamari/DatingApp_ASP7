import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

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
  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    //1. The subscribe method is called on the Observable to start listening for the response
    this.http.get('https://localhost:5001/api/users').subscribe({
      next: response => this.users = response, // when the request is successful, and it receives the response from the server.
      error: error => console.log(error), //  if an error occurs during the request
      complete: () => console.log("Request is complete") // when the request is complete, regardless of success or failure.
    })
  }

}
