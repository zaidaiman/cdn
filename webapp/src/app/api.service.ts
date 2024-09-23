import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import { Observable } from 'rxjs'
import { environment } from '../environments/environment'

export interface RegisterUser {
  username: string
  email: string
  phoneNumber: string
  skillsets: string[]
  hobby: string[]
}

export interface UpdateUser {
  username: string
  email: string
  phoneNumber: string
  skillsets: string[]
  hobby: string[]
}

@Injectable({ providedIn: 'root' })
export class ApiService {
  private baseUrl = environment.baseUrl

  constructor(private http: HttpClient) {}

  getUser(username: string): Observable<any> {
    return this.http.get(`${this.baseUrl}/v1/user/${username}`)
  }

  searchUsers(query: string, page: number, pageSize: number): Observable<any> {
    const params = { query, page: page.toString(), size: pageSize.toString() }
    return this.http.get(`${this.baseUrl}/v1/user/search`, { params })
  }

  registerUser(data: RegisterUser): Observable<any> {
    return this.http.post(`${this.baseUrl}/v1/user/register`, data)
  }

  updateUser(username: string, data: UpdateUser): Observable<any> {
    return this.http.put(`${this.baseUrl}/v1/user/${username}`, data)
  }

  deleteUser(username: string): Observable<any> {
    return this.http.delete(`${this.baseUrl}/v1/user/${username}`)
  }
}
