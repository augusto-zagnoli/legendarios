import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(route: ActivatedRouteSnapshot): boolean {
    if (!this.authService.isAuthenticated()) {
      this.router.navigate(['/login-adm']);
      return false;
    }

    const requiredRoles = route.data?.['roles'] as number[] | undefined;
    if (requiredRoles && requiredRoles.length > 0) {
      const nivel = this.authService.getNivelPermissao();
      if (!requiredRoles.includes(nivel)) {
        this.router.navigate(['/home-adm']);
        return false;
      }
    }

    return true;
  }
}
