import { inject, Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve } from '@angular/router';
import { Observable, of } from 'rxjs';
import { StatDto } from '../../../dtos/stat.dto';
import { StatService } from '../../../services/stat.service';

@Injectable({ providedIn: 'root' })
export class StatsResolver implements Resolve<StatDto[]> {
  private statService = inject(StatService);

  resolve(route: ActivatedRouteSnapshot): Observable<StatDto[]> {
    const exerciseId = Number(route.paramMap.get('exerciseId'));

    const cachedStats = this.statService.tryGetCachedStats(exerciseId);
    if (cachedStats) {
      return of(cachedStats);
    } else {
      return this.statService.getExerciseStats(exerciseId);
    }
  }
}
