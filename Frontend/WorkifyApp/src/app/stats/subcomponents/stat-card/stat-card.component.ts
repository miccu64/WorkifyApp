import { ChangeDetectionStrategy, Component, inject, input, output } from '@angular/core';
import { MatCard, MatCardContent } from '@angular/material/card';
import { StatDto } from '../../../dtos/stat.dto';
import { MatDialog } from '@angular/material/dialog';
import { CreateEditStatFormComponent } from '../create-edit-stat/create-edit-stat-form.component';

@Component({
  selector: 'app-stat-card',
  imports: [MatCard, MatCardContent],
  templateUrl: './stat-card.component.html',
  styleUrls: ['./stat-card.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class StatCardComponent {
  readonly stat = input.required<StatDto>();
  readonly statEdited = output<void>();

  private dialog = inject(MatDialog);

  editStat(): void {
    const dialogRef = this.dialog.open(CreateEditStatFormComponent, {
      data: { exerciseId: this.stat().exerciseId, stat: this.stat() }
    });
    dialogRef.afterClosed().subscribe(() => {
      this.statEdited.emit();
    });
  }
}
