import { Component, input } from '@angular/core';
import { MatCard } from '@angular/material/card';

@Component({
  selector: 'app-info-table',
  imports: [MatCard],
  templateUrl: './info-table.component.html',
  styleUrl: './info-table.component.scss'
})
export class InfoTableComponent {
  title = input.required<string>();
  data = input.required<Map<string, string>>();
}
