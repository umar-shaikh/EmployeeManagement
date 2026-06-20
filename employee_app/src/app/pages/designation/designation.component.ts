
import { Component, inject, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { MasterService } from '../../services/master.service';

import { DesignationModel } from '../../models/Designation.model';
import { DepartmentModel } from '../../models/Department.model';

@Component({
  selector: 'app-designation',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './designation.component.html',
  styleUrls: ['./designation.component.css']
})
export class DesignationComponent implements OnInit {

  masterService = inject(MasterService);

  designationList: DesignationModel[] = [];
  deptList: DepartmentModel[] = [];

  newDesignationObj: DesignationModel = new DesignationModel();

  ngOnInit(): void {

    this.getAllDesignation();
    this.getAllDepartments();

  }

  // =========================
  // GET ALL DESIGNATIONS
  // =========================

  getAllDesignation() {

    this.masterService.getAllDesignation().subscribe({

      next: (res: any) => {

        this.designationList = res;

      },

      error: (err: any) => {

        alert(err.error.message);

      }

    });

  }

  // =========================
  // GET ALL DEPARTMENTS
  // =========================

  getAllDepartments() {

    this.masterService.getAllDept().subscribe({

      next: (res: any) => {

        this.deptList = res;

      },

      error: (err: any) => {

        alert(err.error.message);

      }

    });

  }

  getDepartmentName(deptId: number): string {

  const dept = this.deptList.find(x => x.departmentId == deptId);

  return dept ? dept.departmentName : '';

}

  // =========================
  // SAVE DESIGNATION
  // =========================

  saveDesignation() {

    this.masterService.saveDesignation(this.newDesignationObj).subscribe({

      next: (res: any) => {

        alert('Designation saved successfully');

        this.getAllDesignation();

        this.resetDesignation();

      },

      error: (err: any) => {

        alert(err.error.message);

      }

    });

  }

  // =========================
  // EDIT
  // =========================

  onEdit(item: DesignationModel) {

    const strObj = JSON.stringify(item);

    const parsedObj = JSON.parse(strObj);

    this.newDesignationObj = parsedObj;

  }

  // =========================
  // UPDATE
  // =========================

  updateDesignation() {

    this.masterService.updateDesignation(this.newDesignationObj).subscribe({

      next: (res: any) => {

        alert('Designation updated successfully');

        this.getAllDesignation();

        this.resetDesignation();

      },

      error: (err: any) => {

        alert(err.error.message);

      }

    });

  }

  // =========================
  // DELETE
  // =========================

  onDelete(id: number) {

    const isDelete = confirm('Are you sure you want to delete this designation?');

    if (isDelete) {

      this.masterService.deleteDesignation(id).subscribe({

        next: (res: any) => {

          alert('Designation deleted successfully');

          this.getAllDesignation();

        },

        error: (err: any) => {

          alert(err.error.message);

        }

      });

    }

  }

  // =========================
  // RESET
  // =========================

  resetDesignation() {

    this.newDesignationObj = new DesignationModel();

  }

}