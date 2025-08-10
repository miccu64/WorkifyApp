import { ChangeDetectionStrategy, ChangeDetectorRef, Component, inject, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { PlanDto } from '../../dtos/plan.dto';
import { ListComponent } from '../../layout/list/list.component';
import { WorkoutService } from '../../services/workout.service';
import { CreateEditPlanFormComponent } from '../subcomponents/create-edit-plan/create-edit-plan-form.component';
import { PlanCardComponent } from '../subcomponents/plan-card/plan-card.component';

@Component({
  selector: 'app-plans-list',
  imports: [ListComponent, PlanCardComponent],
  templateUrl: './plans-list.component.html',
  styleUrl: './plans-list.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PlansListComponent implements OnInit {
  plans: PlanDto[] = [];

  private workoutService = inject(WorkoutService);
  private dialog = inject(MatDialog);
  private changeDetectorRef = inject(ChangeDetectorRef);

  ngOnInit(): void {
    this.refreshData();
  }

  openCreateForm(): void {
    const dialogRef = this.dialog.open(CreateEditPlanFormComponent);
    dialogRef.afterClosed().subscribe(() => {
      this.refreshData();
    });
  }

  private refreshData(): void {
    this.plans = this.workoutService.plans.sort((a, b) =>
      a.name.toLocaleLowerCase() > b.name.toLocaleLowerCase() ? 1 : -1
    );

    this.changeDetectorRef.markForCheck();
  }
}
