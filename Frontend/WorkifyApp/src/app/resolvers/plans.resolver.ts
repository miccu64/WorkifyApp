import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { Observable } from 'rxjs';
import { WorkoutService } from '../services/workout.service';
import { PlanDto } from '../dtos/plan.dto';

@Injectable({ providedIn: 'root' })
export class PlansResolver implements Resolve<Observable<PlanDto[]>> {
  constructor(private workoutService: WorkoutService) {}

  resolve(): Observable<PlanDto[]> {
    return this.workoutService.getPlans();
  }
}
