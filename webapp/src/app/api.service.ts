import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import { Observable } from 'rxjs'
import { environment } from '../environments/environment'

@Injectable({ providedIn: 'root' })
export class ApiService {
  private baseUrl = environment.baseUrl

  constructor(private http: HttpClient) {}

  getUser(userId: string): Observable<any> {
    return this.http.get(`${this.baseUrl}/v1/user/${userId}`)
  }

  searchUsers(query: string, page: number, pageSize: number): Observable<any> {
    const params = { query, page: page.toString(), size: pageSize.toString() }
    return this.http.get(`${this.baseUrl}/v1/user/search`, { params })
  }

  registerUser(user: { username: string; email: string }): Observable<any> {
    return this.http.post(`${this.baseUrl}/v1/user/register`, user)
  }

  updateUser(username: string, email: string): Observable<any> {
    return this.http.put(`${this.baseUrl}/v1/user/${username}`, { email })
  }
}
