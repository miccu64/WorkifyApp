import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavbarComponent } from '../navbar/navbar.component';

@Component({
  selector: 'app-base',
  imports: [RouterOutlet, NavbarComponent],
  templateUrl: './base.component.html',
  styleUrl: './base.component.scss'
})
export class BaseComponent {}
