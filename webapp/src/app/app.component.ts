import { Component } from '@angular/core'
import { RouterOutlet } from '@angular/router'
import { FormsModule } from '@angular/forms'
import { ApiService } from './api.service'
import { AuthService } from './auth.service'
import { CommonModule } from '@angular/common'
import { MatInputModule } from '@angular/material/input'
import { MatButtonModule } from '@angular/material/button'
import { MatCardModule } from '@angular/material/card'
import { MatFormFieldModule } from '@angular/material/form-field'
import { MatIconModule } from '@angular/material/icon'
import { MatDividerModule } from '@angular/material/divider'

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    FormsModule,
    CommonModule,
    MatInputModule,
    MatButtonModule,
    MatCardModule,
    MatFormFieldModule,
    MatIconModule,
    MatDividerModule,
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent {
  title = 'webapp'

  userId: string = ''
  username: string = ''
  searchQuery: string = ''
  page: number = 1
  size: number = 25
  newUserRequest = { username: '', email: '' }
  updateUserRequest = { username: '', email: '' }

  tokenResponse: any
  userResponse: any
  searchResponse: any
  updateResponse: any

  constructor(private apiService: ApiService, private authService: AuthService) {}

  getToken() {
    this.authService.auth(this.username).subscribe(response => {
      this.tokenResponse = response
      console.log('Token:', response)
    })
  }

  getUser() {
    this.apiService.getUser(this.userId).subscribe(response => {
      this.userResponse = response
      console.log('User:', response)
    })
  }

  searchUsers() {
    this.apiService.searchUsers(this.searchQuery, this.page, this.size).subscribe(response => {
      this.searchResponse = response
      console.log('Search Results:', response)
    })
  }

  registerUser() {
    this.apiService.registerUser(this.newUserRequest).subscribe(response => {
      console.log('User Registered:', response)
    })
  }

  updateUser() {
    this.apiService.updateUser(this.updateUserRequest.username, this.updateUserRequest.email).subscribe(response => {
      this.updateResponse = response
      console.log('User Updated:', response)
    })
  }
}
