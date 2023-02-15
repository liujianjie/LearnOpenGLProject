#include <glad/glad.h>
#include <GLFW/glfw3.h>
#include <iostream>

// �����¼�
void processInput(GLFWwindow* window)
{
    if (glfwGetKey(window, GLFW_KEY_ESCAPE) == GLFW_PRESS)
        glfwSetWindowShouldClose(window, true);
}

void framebuffer_size_callback(GLFWwindow* window, int width, int height)
{
    glViewport(0, 0, width, height);
}
int main()
{
    glfwInit();
    glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3);// ���汾��(Major) �汾
    glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 3);// �ΰ汾��(Minor)
    glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);// ����ģʽ(Core-profile)
    //glfwWindowHint(GLFW_OPENGL_FORWARD_COMPAT, GL_TRUE);

    // ���Ǵ���
    GLFWwindow* window = glfwCreateWindow(800, 600, "LearnOpenGL", NULL, NULL);
    if (window == NULL)
    {
        std::cout << "Failed to create GLFW window" << std::endl;
        glfwTerminate();
        return -1;
    }
    glfwMakeContextCurrent(window);// �����Ǵ��ڵ�����������Ϊ��ǰ�̵߳���������

    // glad����OpenGL������glfw�ṩOpenGL����ָ���ַ
    if (!gladLoadGLLoader((GLADloadproc)glfwGetProcAddress))
    {
        std::cout << "Failed to initialize GLAD" << std::endl;
        return -1;
    }
    // OpenGL��Ⱦ���ڵĳߴ��С�����ӿ�(Viewport)
    glViewport(0, 0, 800, 600);// 0 0 ���½�,800 600 ���
    // ���ô��ڸı��ӿ�Ҳ�ı�
    glfwSetFramebufferSizeCallback(window, framebuffer_size_callback);
    // ѭ����Ⱦ
    while (!glfwWindowShouldClose(window))// ���һ��GLFW�Ƿ�Ҫ���˳�
    {
        glClearColor(0.2f, 0.3f, 0.3f, 1.0f);
        glClear(GL_COLOR_BUFFER_BIT);
        glfwSwapBuffers(window);// �����ύ����ɫ���壺��������ܻ������Ļ��˸
        glfwPollEvents();// ���������û�д���ʲô�¼�
        processInput(window);
    }
    // �ͷ�/ɾ��֮ǰ�ķ����������Դ
    glfwTerminate();
    return 0;
}