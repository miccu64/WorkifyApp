import { ChangeDetectionStrategy, Component, Inject, inject } from '@angular/core';
import { MatButton } from '@angular/material/button';
import { MatIcon, MatIconModule } from '@angular/material/icon';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-settings',
  imports: [MatIconModule, MatButton, MatIcon],
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class SettingsComponent {
  private authService = inject(AuthService);
  private toastr = inject(ToastrService);
  private router = inject(Router);

  deleteAccount(): void {
    if (confirm('Are you sure you want to delete account?')) {
      this.authService.deleteAccount().subscribe(() => {
        this.authService.clearToken();

        this.toastr.success('Account deleted');

        this.router.navigate(['auth/login']);
      });
    }
  }
}
