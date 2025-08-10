import { ChangeDetectionStrategy, Component, input } from '@angular/core';

@Component({
  selector: 'app-info-table',
  templateUrl: './info-table.component.html',
  styleUrl: './info-table.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class InfoTableComponent {
  readonly title = input.required<string>();
  readonly name = input.required<string>();
  readonly data = input.required<Map<string, string>>();
}
