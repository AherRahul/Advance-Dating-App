import { Injectable } from '@angular/core';
import { User } from '../_model/user';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AuthService } from '../_services/auth.service';

@Injectable()
export class MemberEditResolver implements Resolve<User> {
    constructor(
        private userService: UserService,
        private router: Router,
        private alertify: AlertifyService,
        private authService: AuthService
    ) {}

    resolve(route: ActivatedRouteSnapshot): Observable<User> {
        return this.userService.getuser(this.authService.decodedToken.nameid).pipe(
            catchError(() => {
                this.alertify.error('Problem in retriving Data');
                this.router.navigate(['/members']);
                return of(null);
            })
        );
    }
}
