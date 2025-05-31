import { inject, Injectable } from '@angular/core';
import { ApiService } from '../services/api.service';
import { RegisterDto } from './dtos/register.dto';
import { Observable, tap } from 'rxjs';
import { LogInDto } from './dtos/log-in.dto';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private apiService = inject(ApiService);

  register(dto: RegisterDto): Observable<number> {
    return this.apiService.post<number>('auth/register', dto);
  }

  login(dto: LogInDto): Observable<string> {
    return this.apiService.post<string>('auth/login', dto).pipe(tap(jwtToken => (this.apiService.jwtToken = jwtToken)));
  }
}
