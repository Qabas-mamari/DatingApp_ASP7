import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, catchError } from 'rxjs';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private router: Router, private toastr: ToastrService) { }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error) {
          switch (error.status) {

            //Bad Request and Validation Error
            case 400:
              //1. Checks if the error.error.errors property exists in the error object.
              //1.1. This property is expected to contain the validation errors returned by the server.
              if (error.error.errors) {
                //2. Initializes an empty array modelStateErrors to store the error messages.
                const modelStateErrors = [];
                //3. Iterates over each key in the error.error.errors object using a for...in loop.
                for (const key in error.error.errors) {
                  //4. This condition is used to ensure that the error message exists for the corresponding key.
                  if (error.error.errors[key]) {
                    //5. If the error message exists, it pushes the error message to the modelStateErrors array.
                    modelStateErrors.push(error.error.errors[key])
                  }
                }
                //6. This allows the error to be caught by a higher-level error handler or intercepted by another interceptor.
                throw modelStateErrors;
              } else {
                // there are no model state errors.
                this.toastr.error(error.error, error.status.toString());
              }
              break;

            case 401:
              this.toastr.error("Unauthorized", error.status.toString());
              break;

            case 404:
              this.router.navigateByUrl('/not-found');
              break;

            case 500:
              // This object allows passing additional state information during navigation.
              const navigationExtras: NavigationExtras = { state: error.error };
              this.router.navigateByUrl('/server-error', navigationExtras);
              break;

            default:
              this.toastr.error("Something unexpected went wrong");
              console.log(error);
              break;
          }
        }
        throw error;
      })
    );
  }
}
