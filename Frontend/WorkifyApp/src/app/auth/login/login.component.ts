import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButton, MatButtonModule, MatFabButton } from '@angular/material/button';
import { MatCard, MatCardContent, MatCardTitle } from '@angular/material/card';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatIcon } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { Router } from '@angular/router';
import { AuthService } from '../auth.service';
import { LogInDto } from '../dtos/log-in.dto';
import { ToastrService } from 'ngx-toastr';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
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
  ]
})
export class LoginComponent {
  loginForm: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private authService: AuthService,
    private toastr: ToastrService
  ) {
    this.loginForm = this.formBuilder.group({
      login: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  logIn(): void {
    if (!this.loginForm.valid) {
      return;
    }

    const dto: LogInDto = this.loginForm.value;
    this.authService.login(dto).subscribe({
      next: () => this.router.navigate(['plans']),
      error: (e: HttpErrorResponse) => {
        this.toastr.error(e.status === 401 ? 'Wrong login or password.' : e.message);
      }
    });
  }

  btnRegisterClick(): void {
    this.router.navigate(['auth/register']);
  }
}
