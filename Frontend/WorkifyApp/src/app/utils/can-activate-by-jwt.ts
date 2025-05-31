import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { ApiService } from '../services/api.service';
import { ToastrService } from 'ngx-toastr';

export const canActivateByJwt: CanActivateFn = () => {
  const isTokenGiven = inject(ApiService).jwtToken != null;
  if (isTokenGiven) {
    return true;
  } else {
    inject(ToastrService).error('You are not logged in!');
    inject(Router).navigate(['auth/login']);
    return false;
  }
};
