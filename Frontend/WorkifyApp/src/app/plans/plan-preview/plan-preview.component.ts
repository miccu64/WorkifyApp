import { Component, inject, OnInit } from '@angular/core';
import { PlanDto } from '../../dtos/plan.dto';
import { ActivatedRoute, Router } from '@angular/router';
import { WorkoutService } from '../../services/workout.service';
import { ExerciseDto } from '../../dtos/exercise.dto';
import { ListComponent } from '../../layout/list/list.component';
import { ExerciseCardComponent } from '../../exercises/subcomponents/exercise-card/exercise-card.component';
import { InfoTableComponent } from '../../layout/info-table/info-table.component';
import { MatButtonModule, MatFabButton } from '@angular/material/button';
import { MatIcon, MatIconModule } from '@angular/material/icon';
import { MatDialog } from '@angular/material/dialog';
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
  styleUrls: ['./plan-preview.component.scss']
})
export class PlanPreviewComponent implements OnInit {
  plan!: PlanDto;
  exercises!: ExerciseDto[];
  tableData!: Map<string, string>;

  private activatedRoute = inject(ActivatedRoute);
  private workoutService = inject(WorkoutService);
  private dialog = inject(MatDialog);
  private router = inject(Router);

  ngOnInit(): void {
    const planId = Number(this.activatedRoute.snapshot.paramMap.get('planId'));
    this.refreshData(planId);
  }

  editPlan(): void {
    const dialogRef = this.dialog.open(CreateEditPlanFormComponent, { data: { plan: this.plan } });
    dialogRef.afterClosed().subscribe((refreshedPlans: PlanDto[]) => {
      if (refreshedPlans) {
        this.workoutService.plans = refreshedPlans;

        this.refreshData(this.plan.id);
      }
    });
  }

  deletePlan(): void {
    if (confirm('Are you sure to delete plan "' + this.plan.name + '"?')) {
      this.workoutService.deletePlan(this.plan.id).subscribe((planId: number) => {
        this.workoutService.plans = this.workoutService.plans.filter(p => p.id !== planId);

        this.router.navigate(['/app/plans/list']);
      });
    }
  }

  private refreshData(planId: number): void {
    const plan = this.workoutService.plans.find(p => p.id === planId);
    if (!plan) {
      throw new Error('Plan not found');
    }

    this.plan = plan;
    this.exercises = this.workoutService.exercises.filter(e => plan.exercisesIds.includes(e.id));
    this.tableData = new Map<string, string>([
      ['Name', this.plan.name],
      ['Description', this.plan.description ?? '-'],
      ['Exercises', this.plan.exercisesIds.length.toString()]
    ]);
  }
}
