import { ChangeDetectionStrategy, Component, inject, input } from '@angular/core';
import { Router } from '@angular/router';
import { StatDto } from '../../../dtos/stat.dto';
import { MatCard, MatCardContent } from '@angular/material/card';

@Component({
  selector: 'app-stat-card',
  imports: [MatCard, MatCardContent],
  templateUrl: './stat-card.component.html',
  styleUrls: ['./stat-card.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class StatCardComponent {
  readonly stat = input<StatDto>();

  private router = inject(Router);

  editStat(): void {}
}
