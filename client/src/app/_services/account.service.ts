import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = "https://localhost:5001/api/";

  //Property that is initialized with a new instance of the BehaviorSubject class.
  //The BehaviorSubject is a type of observable that stores the most recent value emitted and allows new subscribers to immediately receive that value.
  private currentUserSource = new BehaviorSubject<User | null>(null);

  //Property that exposes an observable stream of values emitted by currentUserSource.
  //The $ suffix is a convention in RxJS to indicate that it represents an observable.
  currentUser$ = this.currentUserSource.asObservable()

  constructor(private http: HttpClient) { }

  /**
   * TypeScript method for handling a login request.
   * @param model : an object containing the login information.
   * @returns The post method returns an observable that emits the server's response.
   */
  login(model: any){
    //1. The this.http.post<User>(...) indicates that the response from the server is expected to be of type User.
    //2. The pipe operator allows you to chain multiple operators to process the emitted values from the observable.
    return this.http.post<User>(this.baseUrl + 'account/login', model).pipe(
      map((response: User) => { // the response is assigned to a user variable.
        const user = response;
      //If the user exists
        if(user){
          //it is stored in the localStorage as a JSON string representation using JSON.stringify(user)
          localStorage.setItem('user', JSON.stringify(user))
          this.currentUserSource.next(user);
        }
      })
    )
  }

  register(model: any){
    return this.http.post<User>(this.baseUrl + 'account/register', model).pipe(
      map(user=> {
        if(user){
          localStorage.setItem('user', JSON.stringify(user))
          this.currentUserSource.next(user);
        }
      //  return user;
      })
    )
  }

  /**
   * TypeScript method updates the current user information.
   * @param user : represents the new user information.
   */
  setCurrentUser(user : User){
    this.currentUserSource.next(user);
  }

  /**
   * TypeScript method for handling the logout functionality.
   * the method removes the 'user' item from the localStorage, effectively clearing any previously stored user information.ds
   */
  logout(){
   localStorage.removeItem('user');
   this.currentUserSource.next(null);
  }
}
