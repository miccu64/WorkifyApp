import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class ApiService {
  private httpClient: HttpClient = inject(HttpClient);

  private readonly jwtTokenStorageName = 'JwtToken';
  private _jwtToken: string | null = localStorage.getItem(this.jwtTokenStorageName);
  public get jwtToken(): string | null {
    return this._jwtToken;
  }
  public set jwtToken(value: string | null) {
    this._jwtToken = value;
    if (value) {
      localStorage.setItem(this.jwtTokenStorageName, value);
    } else {
      localStorage.removeItem(this.jwtTokenStorageName);
    }
  }

  private readonly baseUrl = 'http://192.168.100.29:5030/api/';

  private get headers(): HttpHeaders {
    return new HttpHeaders().set('Authorization', 'Bearer ' + this.jwtToken);
  }

  get<T>(url: string, params?: HttpParams): Observable<T> {
    return this.httpClient.get<T>(this.baseUrl + url, {
      headers: this.headers,
      params
    });
  }

  post<T>(url: string, body?: object, params?: HttpParams, isPlainTextResponse?: boolean): Observable<T> {
    return this.httpClient.post<T>(this.baseUrl + url, body, {
      headers: this.headers,
      params,
      responseType: (isPlainTextResponse ? 'text' : 'json') as 'json'
    });
  }

  put<T>(url: string, body?: object, params?: HttpParams): Observable<T> {
    return this.httpClient.put<T>(this.baseUrl + url, body, { headers: this.headers, params });
  }

  delete<T>(url: string, params?: HttpParams): Observable<T> {
    return this.httpClient.delete<T>(this.baseUrl + url, { headers: this.headers, params });
  }
}
