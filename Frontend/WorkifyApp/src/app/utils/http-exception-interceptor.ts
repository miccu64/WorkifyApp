import { HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { HttpInterceptorFn } from '@angular/common/http';

export const httpExceptionInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const toastr = inject(ToastrService);

  return next(req).pipe(
    catchError(error => {
      if (error instanceof HttpErrorResponse) {
        if (error.error instanceof ErrorEvent) {
          console.error('Client-side error:', error.error.message);
          toastr.error('A client-side error occurred. Please try again.', 'Error');
        } else {
          console.warn(`Backend returned code ${error.status}, body was:`, error.error);

          let message = '';

          switch (error.status) {
            case 0:
              message = 'Cannot connect to the server. Check your network.';
              break;
            case 400:
              message = 'Bad request. Please check your input.';
              break;
            case 401:
              message = 'You are not authorized. Please log in.';
              router.navigate(['login']);
              break;
            case 403:
              message = 'You do not have permission to perform this action.';
              break;
            case 404:
              message = 'Requested resource not found.';
              break;
            case 500:
              message = 'Server encountered an error. Try again later.';
              break;
            default:
              message = 'An unexpected error occurred.';
              break;
          }

          const errorMessage = formatServerError(error);
          if (errorMessage) {
            message += ' ' + JSON.stringify(errorMessage);
          }
          toastr.error(message, 'Error');
        }
      } else {
        console.error('Unknown error occurred:', error);
        toastr.error('An unknown error occurred. Please try again.', 'Error');
      }

      return throwError(() => error);
    })
  );
};

function formatServerError(error: HttpErrorResponse): string {
  let errorObj = error.error;

  if (!error.error) {
    return '';
  }

  try {
    errorObj = JSON.parse(errorObj);

    if (errorObj.title && errorObj.status) {
      return errorObj.title;
    }

    if (errorObj.message) {
      return errorObj.message;
    }

    if (Array.isArray(errorObj.errors)) {
      return errorObj.errors.join(', ');
    }

    return JSON.stringify(errorObj);
  } catch {
    return errorObj;
  }
}
