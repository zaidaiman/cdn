import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import { Observable, tap } from 'rxjs'
import { environment } from '../environments/environment'

@Injectable({ providedIn: 'root' })
export class AuthService {
  private baseUrl = environment.baseUrl

  constructor(private http: HttpClient) {}

  auth(username: string): Observable<any> {
    return this.http.get(`${this.baseUrl}/v1/auth/token/${username}`).pipe(
      tap((response: any) => {
        const token = response.token
        if (token) {
          localStorage.setItem('authToken', token)
        }
      }),
    )
  }

  getToken(): string | null {
    return localStorage.getItem('authToken')
  }

  clearToken(): void {
    localStorage.removeItem('authToken')
  }
}
