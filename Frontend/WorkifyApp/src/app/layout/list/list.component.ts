import { ChangeDetectionStrategy, Component, input, output } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';

@Component({
  selector: 'app-list',
  imports: [MatListModule, MatIconModule, MatButtonModule, MatCardModule],
  templateUrl: './list.component.html',
  styleUrl: './list.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ListComponent {
  readonly title = input.required<string>();
  readonly fabClicked = output<void>();
  readonly fabHidden = input<boolean>();

  onFabClicked(): void {
    this.fabClicked.emit();
  }
}
