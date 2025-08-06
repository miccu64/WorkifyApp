import { Component, input } from '@angular/core';
import { MatCard } from '@angular/material/card';

@Component({
  selector: 'app-info-table',
  imports: [MatCard],
  templateUrl: './info-table.component.html',
  styleUrl: './info-table.component.scss'
})
export class InfoTableComponent {
  readonly title = input.required<string>();
  readonly data = input.required<Map<string, string>>();
}
