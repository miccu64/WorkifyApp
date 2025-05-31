import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButton, MatButtonModule, MatFabButton } from '@angular/material/button';
import { MatCard, MatCardContent, MatCardTitle } from '@angular/material/card';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatIcon } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { Router } from '@angular/router';
import { AuthService } from '../auth.service';
import { RegisterDto } from '../dtos/register.dto';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
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
export class RegisterComponent {
  registerForm: FormGroup;

  constructor(private formBuilder: FormBuilder, private router: Router, private authService: AuthService) {
    this.registerForm = this.formBuilder.group({
      login: ['', Validators.compose([Validators.required, Validators.minLength(6)])],
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.compose([Validators.required, Validators.minLength(6)])]
    });
  }

  register(): void {
    if (!this.registerForm.valid) {
      return;
    }

    const dto: RegisterDto = this.registerForm.value;
    console.log(dto);
    this.authService.register(dto).subscribe(() => this.router.navigate(['auth/login']));
  }

  btnLoginClick(): void {
    this.router.navigate(['auth/login']);
  }
}
