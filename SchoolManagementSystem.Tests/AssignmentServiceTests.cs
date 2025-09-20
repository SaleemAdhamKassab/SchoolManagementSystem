using Microsoft.AspNetCore.Http;
using Moq;
using SchoolManagementSystem.Application.Contracts.Assignment.Request;
using SchoolManagementSystem.Application.Services;
using SchoolManagementSystem.Core.Entities;
using SchoolManagementSystem.Core.Interfaces.Repositories;

public class AssignmentServiceTests
{
	private readonly Mock<IAssignmentRepository> _assignmentRepositoryMock;
	private readonly Mock<ICourseRepository> _courseRepositoryMock;
	private readonly Mock<IAuthRepository> _authRepositoryMock;
	private readonly AssignmentService _assignmentService;

	public AssignmentServiceTests()
	{
		_assignmentRepositoryMock = new Mock<IAssignmentRepository>();
		_courseRepositoryMock = new Mock<ICourseRepository>();
		_authRepositoryMock = new Mock<IAuthRepository>();

		_assignmentService = new AssignmentService(
			_courseRepositoryMock.Object,
			_assignmentRepositoryMock.Object,
			_authRepositoryMock.Object
		);
	}

	[Fact]
	public async Task AddAssignmentAsync_ValidRequest_ReturnsSuccessResponse()
	{
		// Arrange
		var teacherId = 123;
		var courseId = 1;
		var request = new CreateAssignmentRequest
		{
			Title = "Test Assignment",
			Description = "Test Description",
			DueDate = DateTime.Now.AddDays(7),
			CourseId = courseId
		};
		var teacher = new User { Id = teacherId, FirstName = "John", LastName = "Doe" };
		var course = new Course { Id = courseId, TeacherId = teacherId };
		var createdAssignment = new Assignment
		{
			Id = 1,
			Title = request.Title,
			Description = request.Description,
			DueDate = request.DueDate,
			CourseId = request.CourseId
		};

		_authRepositoryMock
			.Setup(repo => repo.GetUserByIdAsync(teacherId, It.IsAny<CancellationToken>()))
			.ReturnsAsync(teacher);

		_courseRepositoryMock
			.Setup(repo => repo.GetCourseByIdAsync(courseId, It.IsAny<CancellationToken>()))
			.ReturnsAsync(course);

		// mock the repository to return null for the course
		//_courseRepositoryMock
		//	.Setup(repo => repo.GetCourseByIdAsync(courseId, It.IsAny<CancellationToken>()))
		//	.ReturnsAsync((Course)null); // Explicitly returning null

		_assignmentRepositoryMock
			.Setup(repo => repo.AddAssignment(It.IsAny<Assignment>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(createdAssignment);

		// Act
		var result = await _assignmentService.AddAssignmentAsync(request, teacherId, CancellationToken.None);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.Success);
		Assert.Equal("Assignment created successfully", result.Message);
		Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
		Assert.NotNull(result.Data);
		Assert.Equal(createdAssignment.Id, result.Data.Id);

		_authRepositoryMock.Verify(repo => repo.GetUserByIdAsync(teacherId, It.IsAny<CancellationToken>()), Times.Once);
		_courseRepositoryMock.Verify(repo => repo.GetCourseByIdAsync(courseId, It.IsAny<CancellationToken>()), Times.Once);
		_assignmentRepositoryMock.Verify(repo => repo.AddAssignment(It.IsAny<Assignment>(), It.IsAny<CancellationToken>()), Times.Once);
	}
}