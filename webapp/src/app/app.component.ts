import { Component } from '@angular/core'
import { RouterOutlet } from '@angular/router'
import { FormsModule } from '@angular/forms'
import { ApiService, RegisterUser, UpdateUser } from './api.service'
import { AuthService } from './auth.service'
import { CommonModule } from '@angular/common'
import { MatInputModule } from '@angular/material/input'
import { MatButtonModule } from '@angular/material/button'
import { MatCardModule } from '@angular/material/card'
import { MatFormFieldModule } from '@angular/material/form-field'
import { MatIconModule } from '@angular/material/icon'
import { MatDividerModule } from '@angular/material/divider'
import { MatChipInputEvent, MatChipsModule } from '@angular/material/chips'
import { COMMA, ENTER } from '@angular/cdk/keycodes'

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
    MatChipsModule,
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent {
  separatorKeysCodes: number[] = [ENTER, COMMA]
  username: string = ''
  searchQuery: string = ''
  page: number = 1
  size: number = 5
  newUserRequest = { username: '', email: '', phoneNumber: '', skillsets: [] as string[], hobby: [] as string[] } as RegisterUser
  updateUserRequest = { username: '', email: '', phoneNumber: '', skillsets: [] as string[], hobby: [] as string[] } as UpdateUser

  tokenResponse: any
  userResponse: any
  searchResponse: any
  updateResponse: any
  registerResponse: any
  deleteResponse: any

  constructor(private apiService: ApiService, private authService: AuthService) {}

  getToken() {
    this.authService.auth(this.username).subscribe(response => {
      this.tokenResponse = response
    })
  }

  getUser() {
    this.apiService.getUser(this.username).subscribe(response => {
      this.userResponse = response
      this.updateUserRequest = response
    })
  }

  searchUsers() {
    this.apiService.searchUsers(this.searchQuery, this.page, this.size).subscribe(response => {
      this.searchResponse = response
    })
  }

  registerUser() {
    this.apiService.registerUser(this.newUserRequest).subscribe(response => {
      this.registerResponse = response
    })
  }

  updateUser() {
    this.apiService.updateUser(this.updateUserRequest.username, this.updateUserRequest).subscribe(response => {
      this.updateResponse = response
    })
  }

  deleteUser() {
    this.apiService.deleteUser(this.username).subscribe(response => {
      this.deleteResponse = response
    })
  }

  addSkill(event: MatChipInputEvent, action: string): void {
    const input = event.input
    const value = event.value as string

    if ((value || '').trim()) {
      if (action === 'update') this.updateUserRequest.skillsets.push(value.trim())
      else this.newUserRequest.skillsets.push(value.trim())
    }

    if (input) input.value = ''
  }

  removeSkill(skill: string, action: string): void {
    if (action === 'update') {
      const index = this.updateUserRequest.skillsets.indexOf(skill)
      if (index >= 0) this.updateUserRequest.skillsets.splice(index, 1)
    } else {
      const index = this.newUserRequest.skillsets.indexOf(skill)
      if (index >= 0) this.newUserRequest.skillsets.splice(index, 1)
    }
  }

  addHobby(event: MatChipInputEvent, action: string): void {
    const input = event.input
    const value = event.value as string

    if ((value || '').trim()) {
      if (action === 'update') this.updateUserRequest.hobby.push(value.trim())
      else this.newUserRequest.hobby.push(value.trim())
    }

    if (input) input.value = ''
  }

  removeHobby(skill: string, action: string): void {
    if (action === 'update') {
      const index = this.updateUserRequest.hobby.indexOf(skill)
      if (index >= 0) this.updateUserRequest.hobby.splice(index, 1)
    } else {
      const index = this.newUserRequest.hobby.indexOf(skill)
      if (index >= 0) this.newUserRequest.hobby.splice(index, 1)
    }
  }
}
