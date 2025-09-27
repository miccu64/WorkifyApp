import { Location } from '@angular/common';
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, inject, OnInit } from '@angular/core';
import { MatButtonModule, MatFabButton } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { MatIcon, MatIconModule } from '@angular/material/icon';
import { ActivatedRoute, Router } from '@angular/router';
import { firstValueFrom } from 'rxjs';
import { ExerciseCardComponent } from '../../../components/exercises/subcomponents/exercise-card/exercise-card.component';
import { ExerciseDto } from '../../../dtos/exercise.dto';
import { PlanDto } from '../../../dtos/plan.dto';
import { InfoTableComponent } from '../../../layout/info-table/info-table.component';
import { ListComponent } from '../../../layout/list/list.component';
import { WorkoutService } from '../../../services/workout.service';
import { CreateEditPlanFormComponent } from '../subcomponents/create-edit-plan/create-edit-plan-form.component';

@Component({
  selector: 'app-plan-preview',
  imports: [
    ListComponent,
    ExerciseCardComponent,
    InfoTableComponent,
    MatIcon,
    MatIconModule,
    MatFabButton,
    MatButtonModule
  ],
  templateUrl: './plan-preview.component.html',
  styleUrls: ['./plan-preview.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PlanPreviewComponent implements OnInit {
  plan!: PlanDto;
  exercises!: ExerciseDto[];
  tableData!: Map<string, string>;

  private activatedRoute = inject(ActivatedRoute);
  private workoutService = inject(WorkoutService);
  private dialog = inject(MatDialog);
  private router = inject(Router);
  private location = inject(Location);
  private changeDetectorRef = inject(ChangeDetectorRef);

  ngOnInit(): void {
    const planId = Number(this.activatedRoute.snapshot.paramMap.get('planId'));
    this.refreshData(planId);
  }

  editPlan(): void {
    const dialogRef = this.dialog.open(CreateEditPlanFormComponent, { data: { plan: this.plan } });
    dialogRef.afterClosed().subscribe(() => {
      this.refreshData(this.plan.id);
    });
  }

  async deletePlan(): Promise<void> {
    if (confirm('Are you sure to delete plan "' + this.plan.name + '"?')) {
      await firstValueFrom(this.workoutService.deletePlan(this.plan.id));
      await this.workoutService.refreshPlans();

      await this.router.navigate(['/app/plans/list']);
    }
  }

  goBack(): void {
    this.location.back();
  }

  private refreshData(planId: number): void {
    const plan = this.workoutService.plans.find(p => p.id === planId);
    if (!plan) {
      throw new Error('Plan not found');
    }

    this.plan = plan;
    this.exercises = this.workoutService.exercises.filter(e => plan.exercisesIds.includes(e.id));
    this.tableData = new Map<string, string>([
      ['Description', this.plan.description ?? '-'],
      ['Exercises', this.plan.exercisesIds.length.toString()]
    ]);

    this.changeDetectorRef.markForCheck();
  }
}
