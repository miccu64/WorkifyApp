import { Component, inject, OnInit } from '@angular/core';
import { PlanDto } from '../../dtos/plan.dto';
import { ActivatedRoute } from '@angular/router';
import { WorkoutService } from '../../services/workout.service';
import { ExerciseDto } from '../../dtos/exercise.dto';
import { ListComponent } from '../../layout/list/list.component';
import { ExerciseCardComponent } from '../../exercises/subcomponents/exercise-card/exercise-card.component';

@Component({
  selector: 'app-plan-preview',
  imports: [ListComponent, ExerciseCardComponent],
  templateUrl: './plan-preview.component.html',
  styleUrls: ['./plan-preview.component.scss']
})
export class PlanPreviewComponent implements OnInit {
  plan!: PlanDto;
  exercises!: ExerciseDto[];

  private activatedRoute = inject(ActivatedRoute);
  private workoutService = inject(WorkoutService);

  ngOnInit(): void {
    console.log('a');
    const planId = Number(this.activatedRoute.snapshot.paramMap.get('planId'));
    const plan = this.workoutService.plans.find(p => p.id == planId);
    if (plan) {
      this.plan = plan;
      this.exercises = this.workoutService.exercises.filter(e => plan.exercisesIds.includes(e.id));
    } else {
      throw new Error('Plan not found', { cause: planId });
    }
  }

  assignExercise() {}
}
