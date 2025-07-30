import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from './../../environments/environment';

@Injectable({ providedIn: 'root' })
export class ApiService {
  private httpClient: HttpClient = inject(HttpClient);

  private readonly jwtTokenStorageName = 'JwtToken';
  private _jwtToken: string | null = sessionStorage.getItem(this.jwtTokenStorageName);
  public get jwtToken(): string | null {
    return this._jwtToken;
  }
  public set jwtToken(value: string | null) {
    this._jwtToken = value;
    if (value) {
      sessionStorage.setItem(this.jwtTokenStorageName, value);
    } else {
      sessionStorage.removeItem(this.jwtTokenStorageName);
    }
  }

  private get headers(): HttpHeaders {
    return new HttpHeaders().set('Authorization', 'Bearer ' + this.jwtToken);
  }

  get<T>(url: string, params?: HttpParams): Observable<T> {
    return this.httpClient.get<T>(environment.apiUrl + url, {
      headers: this.headers,
      params
    });
  }

  post<T>(url: string, body?: object, params?: HttpParams, isPlainTextResponse?: boolean): Observable<T> {
    return this.httpClient.post<T>(environment.apiUrl + url, body, {
      headers: this.headers,
      params,
      responseType: (isPlainTextResponse ? 'text' : 'json') as 'json'
    });
  }

  put<T>(url: string, body?: object, params?: HttpParams): Observable<T> {
    return this.httpClient.put<T>(environment.apiUrl + url, body, { headers: this.headers, params });
  }

  delete<T>(url: string, params?: HttpParams): Observable<T> {
    return this.httpClient.delete<T>(environment.apiUrl + url, { headers: this.headers, params });
  }
}
