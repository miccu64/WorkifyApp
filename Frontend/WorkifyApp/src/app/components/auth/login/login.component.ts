import { HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButton, MatButtonModule, MatFabButton } from '@angular/material/button';
import { MatCard, MatCardContent, MatCardTitle } from '@angular/material/card';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatIcon } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { firstValueFrom } from 'rxjs';
import { LogInDto } from '../../../dtos/log-in.dto';
import { AuthService } from '../../../services/auth.service';
import { WorkoutService } from '../../../services/workout.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['../auth-styles.scss'],
  imports: [
    MatFabButton,
    MatButton,
    MatButtonModule,
    MatCard,
    MatCardTitle,
    MatFormField,
    MatLabel,
    MatCardContent,
    ReactiveFormsModule,
    MatInputModule,
    MatIcon
  ],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class LoginComponent {
  loginForm: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private authService: AuthService,
    private toastr: ToastrService,
    private workoutService: WorkoutService
  ) {
    this.loginForm = this.formBuilder.group({
      login: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  async logIn(): Promise<void> {
    if (!this.loginForm.valid) {
      return;
    }

    const dto: LogInDto = this.loginForm.value;

    try {
      await firstValueFrom(this.authService.login(dto));

      await this.workoutService.refreshPlansAndExercises();

      await this.router.navigate(['app/plans/list']);
    } catch (e: unknown) {
      if (e instanceof HttpErrorResponse) {
        if (e.status === 401) {
          this.toastr.error('Wrong login or password.');
        } else {
          this.toastr.error(e.message || 'Login failed.');
        }
      }
    }
  }

  btnRegisterClick(): void {
    this.router.navigate(['auth/register']);
  }
}
