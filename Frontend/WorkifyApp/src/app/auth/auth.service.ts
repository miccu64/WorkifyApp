import { inject, Injectable } from '@angular/core';
import { RegisterDto } from './dtos/register.dto';
import { Observable, tap } from 'rxjs';
import { LogInDto } from './dtos/log-in.dto';
import { ApiService } from '../utils/api.service';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private apiService = inject(ApiService);

  register(dto: RegisterDto): Observable<number> {
    return this.apiService.post<number>('auth/register', dto, undefined, true);
  }

  login(dto: LogInDto): Observable<string> {
    return this.apiService
      .post<string>('auth/login', dto, undefined, true)
      .pipe(tap(jwtToken => (this.apiService.jwtToken = jwtToken)));
  }
}
