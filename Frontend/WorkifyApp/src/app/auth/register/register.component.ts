import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButton, MatButtonModule, MatFabButton } from '@angular/material/button';
import { MatCard, MatCardContent, MatCardTitle } from '@angular/material/card';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatIcon } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { Router } from '@angular/router';

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
  loginForm: FormGroup;

  constructor(private formBuilder: FormBuilder, private router: Router) {
    this.loginForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });
  }

  onSubmit(): void {
    if (this.loginForm.valid) {
      console.log('Login form data:', this.loginForm.value);
    }
  }

  btnLoginClick(): void {
    this.router.navigate(['auth/register']);
  }
}
