import { Injectable } from '@angular/core';
import { User } from '../_model/user';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class MemberDetailResolver implements Resolve<User> {
    constructor(
        private userService: UserService,
        private router: Router,
        private alertify: AlertifyService
    ) {}

    resolve(route: ActivatedRouteSnapshot): Observable<User> {
        return this.userService.getuser(route.params.id).pipe(
            catchError(() => {
                this.alertify.error('Problem in retriving Data');
                this.router.navigate(['/members']);
                return of(null);
            })
        );
    }
}
