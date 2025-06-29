import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from './api.service';

export const canActivateByJwt: CanActivateFn = () => {
  const apiService = inject(ApiService);
  const token = apiService.jwtToken;
  if (token && isTokenValid(token)) {
    return true;
  } else {
    apiService.jwtToken = null;

    inject(ToastrService).error('You are not logged in!');
    inject(Router).navigate(['auth/login']);
    return false;
  }
};

function isTokenValid(token: string): boolean {
  try {
    const expiry = JSON.parse(atob(token.split('.')[1])).exp;
    return Math.floor(new Date().getTime() / 1000) < expiry;
  } catch {
    return false;
  }
}
