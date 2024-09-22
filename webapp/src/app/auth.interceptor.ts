import { Injectable } from '@angular/core'
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest } from '@angular/common/http'
import { Observable } from 'rxjs'
import { AuthService } from './auth.service'

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private authService: AuthService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const exceptionList = ['v1/auth/token', 'v1/user/register']
    if (exceptionList.some(url => req.url.includes(url))) {
      return next.handle(req)
    }

    const authToken = this.authService.getToken()
    if (authToken) {
      const authReq = req.clone({
        setHeaders: { Authorization: `Bearer ${authToken}` },
      })
      return next.handle(authReq)
    }
    return next.handle(req)
  }
}
