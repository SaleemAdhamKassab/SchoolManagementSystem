using Microsoft.AspNetCore.Http;
using SchoolManagementSystem.Application.Contracts.Assignment.Request;
using SchoolManagementSystem.Application.Contracts.Assignment.Response;
using SchoolManagementSystem.Application.Contracts.Common;
using SchoolManagementSystem.Application.Interfaces.Services;
using SchoolManagementSystem.Core.Entities;
using SchoolManagementSystem.Core.Enums;
using SchoolManagementSystem.Core.Interfaces.Repositories;

namespace SchoolManagementSystem.Application.Services
{
	public class AssignmentService(ICourseRepository courseRepository,
		IAssignmentRepository assignmentRepository,
		IAuthRepository authRepository) : IAssignmentService
	{
		private readonly ICourseRepository _courseRepository = courseRepository;
		private readonly IAssignmentRepository _assignmentRepository = assignmentRepository;
		private readonly IAuthRepository _authRepository = authRepository;


		public async Task<GeneralResponse<AssignmentResponse>> AddAssignmentAsync(CreateAssignmentRequest request, int teacherId, CancellationToken cancellationToken)
		{
			var teacher = _authRepository.GetUserByIdAsync(teacherId, cancellationToken);

			if (teacher == null)
				return new GeneralResponse<AssignmentResponse>(false, "Invalid teacher Id", null, StatusCodes.Status404NotFound);

			var course = await _courseRepository.GetCourseByIdAsync(request.CourseId, cancellationToken);

			if (course == null)
				return new GeneralResponse<AssignmentResponse>(false, "Invalid course Id", null, StatusCodes.Status404NotFound);

			if (course.TeacherId != teacherId)
				return new GeneralResponse<AssignmentResponse>(false, "Teacher is not authorized to add assignments to this course", null, StatusCodes.Status403Forbidden);

			var assignment = new Assignment
			{
				Title = request.Title,
				Description = request.Description,
				DueDate = request.DueDate,
				CourseId = request.CourseId
			};

			var createdAssignment = await _assignmentRepository.AddAssignment(assignment, cancellationToken);

			var responseDto = new AssignmentResponse
			{
				Id = createdAssignment.Id,
				Title = createdAssignment.Title,
				Description = createdAssignment.Description,
				DueDate = createdAssignment.DueDate,
				Course = createdAssignment.Title
			};

			return new GeneralResponse<AssignmentResponse>(true, "Assignment created successfully", responseDto, StatusCodes.Status200OK);
		}
		public async Task<GeneralResponse<AssignmentSubmissionResponse>> AddAssignmentSubmissionAsync(AssignmentSubmissionRequest request, int studentId, CancellationToken cancellationToken)
		{
			var student = await _authRepository.GetUserByIdAsync(studentId, cancellationToken);
			if (student == null)
				return new GeneralResponse<AssignmentSubmissionResponse>(false, "Invalid student ID", null, StatusCodes.Status404NotFound);


			var assignment = await _assignmentRepository.GetAssignmentByIdAsync(request.AssignmentId, cancellationToken);
			if (assignment == null)
				return new GeneralResponse<AssignmentSubmissionResponse>(false, "Assignment not found", null, StatusCodes.Status404NotFound);

			if (assignment.DueDate < DateTime.Today)
				return new GeneralResponse<AssignmentSubmissionResponse>(false, "Assignment due date has passed, Submission is not allowed", null, StatusCodes.Status400BadRequest);


			var isEnrolled = await _courseRepository.IsStudentEnrolledInCourseAsync(assignment.CourseId, studentId, cancellationToken);
			if (!isEnrolled)
				return new GeneralResponse<AssignmentSubmissionResponse>(false, "Student is not enrolled in this course", null, StatusCodes.Status403Forbidden);


			var existingSubmission = await _assignmentRepository.GetSubmissionByAssignmentAndStudentAsync(request.AssignmentId, studentId, cancellationToken);
			if (existingSubmission != null)
				return new GeneralResponse<AssignmentSubmissionResponse>(false, "Assignment has already been submitted", null, StatusCodes.Status409Conflict);


			var studentAssignmentSubmission = new StudentAssignmentSubmission
			{
				StudentId = studentId,
				Status = EnmSubmissionStatus.Submitted.ToString(),
				SubmittedDate = DateTime.Now,
				AssignmentId = request.AssignmentId
			};

			var createdStudentAssignmentSubmission = await _assignmentRepository.AddStudentAssignmentSubmission(studentAssignmentSubmission, cancellationToken);

			var responseDto = new AssignmentSubmissionResponse
			{
				Id = createdStudentAssignmentSubmission.Id,
				Status = createdStudentAssignmentSubmission.Status,
				SubmittedDate = createdStudentAssignmentSubmission.SubmittedDate,
				Student = $"{student!.FirstName} {student!.LastName}"
			};

			return new GeneralResponse<AssignmentSubmissionResponse>(true, "Assignment submitted successfully", responseDto, StatusCodes.Status200OK);
		}
		public async Task<GeneralResponse<GradeAssignmentResponse>> GradeAssignmentAsync(GradeAssignmentRequest request, int teacherId, CancellationToken cancellationToken)
		{
			var teacher = await _authRepository.GetUserByIdAsync(teacherId, cancellationToken);
			if (teacher == null)
				return new GeneralResponse<GradeAssignmentResponse>(false, "Invalid teacher ID", null, StatusCodes.Status404NotFound);


			var submission = await _assignmentRepository.GetSubmissionByIdAsync(request.StudentAssignmentSubmissionId, cancellationToken);
			if (submission == null)
				return new GeneralResponse<GradeAssignmentResponse>(false, "Submission not found", null, StatusCodes.Status404NotFound);

			//if (submission.Status == EnmSubmissionStatus.Graded.ToString())
			//	return new GeneralResponse<GradeAssignmentResponse>(false, "The submission is already graded", null, StatusCodes.Status409Conflict);

			var assignment = await _assignmentRepository.GetAssignmentByIdAsync(submission.AssignmentId, cancellationToken);
			if (assignment == null)
				return new GeneralResponse<GradeAssignmentResponse>(false, "Associated assignment not found", null, StatusCodes.Status404NotFound);

			var course = await _courseRepository.GetCourseByIdAsync(assignment.CourseId, cancellationToken);
			if (course == null || course.TeacherId != teacherId)
				return new GeneralResponse<GradeAssignmentResponse>(false, "Teacher is not authorized to grade this assignment", null, StatusCodes.Status403Forbidden);


			submission.Grade = request.Grade;
			submission.Status = EnmSubmissionStatus.Graded.ToString();
			submission.TeacherFeedback = request.TeacherFeedback;
			submission.GradedDate = DateTime.Now;


			bool isSuccess = await _assignmentRepository.UpdateStudentAssignmentSubmission(submission, cancellationToken);

			if (!isSuccess)
				return new GeneralResponse<GradeAssignmentResponse>(false, "Failed to update submission", null, StatusCodes.Status500InternalServerError); ;

			var resultDto = new GradeAssignmentResponse
			{
				Id = submission.Id,
				Assignment = assignment.Title,
				Grade = submission.Grade,
				GradedDate = submission.GradedDate,
				Status = submission.Status,
				SubmittedDate = submission.SubmittedDate,
				TeacherFeedback = submission.TeacherFeedback
			};

			return new GeneralResponse<GradeAssignmentResponse>(true, "Assignment graded successfully", resultDto, StatusCodes.Status200OK);
		}
	}
}