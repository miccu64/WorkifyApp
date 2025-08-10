import { Injectable } from '@angular/core';
import { StatDto } from '../../dtos/stat.dto';
import { ActivatedRouteSnapshot, Resolve } from '@angular/router';
import { StatService } from '../../services/stat.service';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class StatsResolver implements Resolve<StatDto[]> {
  constructor(private statService: StatService) {}
  resolve(route: ActivatedRouteSnapshot): Observable<StatDto[]> {
    const exerciseId = Number(route.paramMap.get('exerciseId'));

    return this.statService.getExerciseStats(exerciseId);
  }
}
