import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from '../utils/api.service';
import { PlanDto } from '../dtos/plan.dto';

@Injectable({ providedIn: 'root' })
export class WorkoutService {
  private apiService = inject(ApiService);

  getPlans(): Observable<PlanDto[]> {
    return this.apiService.get<PlanDto[]>('plans');
  }
}
