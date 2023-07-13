import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-test-error',
  templateUrl: './test-error.component.html',
  styleUrls: ['./test-error.component.css']
})
export class TestErrorComponent implements OnInit {
 baseURl = "https://localhost:5001/api/";
 validationErrors: string[]= [];

 constructor(private http: HttpClient) {}

 ngOnInit() {}

 // 400 Bad Request Error
  get400Error(){
    return this.http.get(this.baseURl+ 'buggy/bad-request').subscribe({
      next: response=> console.log(response),
      error: error=> console.log(error)
    });
  }

  // 401 Unauthorize Error
  get401Error(){
    return this.http.get(this.baseURl+ "buggy/auth").subscribe({
      next: response=> console.log(response),
      error: error => console.log(error)
    });
  }

  // 404 Not Found Error
  get404Error(){
    return this.http.get(this.baseURl+ 'buggy/not-found').subscribe({
      next: response=> console.log(response),
      error: error => console.log(error)
    });
  }

 // 500 Server Error
 get500Error(){
  return this.http.get(this.baseURl+ 'buggy/server-error').subscribe({
    next: response=> console.log(response),
    error: error => console.log(error)
  });
}

 // 400 validation
  get400ValidationError(){
    return this.http.post(this.baseURl+ 'account/register', {}).subscribe({
      next: response=> console.log(response),
      error: error => {
        console.log(error),
        this.validationErrors = error;
      }
    });
  }
}
